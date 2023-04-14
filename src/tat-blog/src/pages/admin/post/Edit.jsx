import { useEffect, useState } from 'react'
import { Button, Form } from 'react-bootstrap'
import { Link, Navigate, useLocation, useNavigate, useParams } from 'react-router-dom'
import { decode, isEmptyOrSpaces, isInteger } from '../../../utils/Utils'
import blogApi from '../../../api/blogApi'

const Edit = () => {
  // state
  const [post, setPost] = useState({
    id: 0,
    title: '',
    shortDescription: '',
    description: '',
    meta: '',
    imageUrl: '',
    category: {},
    author: {},
    tags: [],
    selectedTags: '',
    published: false
  })

  const [filter, setFilter] = useState({
    authorList: [],
    categoryList: []
  })

  const location = useLocation()
  const navigate = useNavigate()

  // get from params
  const { id } = useParams()
  console.log('id: ', id)

  // Call API
  useEffect(() => {
    document.title = 'Thêm/Cập nhật bài viết'
    ;(async () => {
      try {
        const filterResponse = await blogApi.getFilter()
        const response = await blogApi.getById(id)
        if (response.isSuccess && filterResponse) {
          const data = response.result
          const filterdata = filterResponse.result
          setFilter({ authorList: filterdata.authorList, categoryList: filterdata.categoryList })
          setPost({ ...data, selectedTags: data.tags.map((tag) => tag?.name).join('\r\n') })
        }
      } catch (error) {
        console.log(error)
      }
    })()
  }, [id])

  // Method handle event
  const handleSubmit = (e) => {
    e.preventDefault()

    console.log(e.target)
    console.log('>>> check post submit: ', post)

    let formData = new FormData(e.target)
    formData.append('published', post.published)
    formData.forEach((x) => console.log(x))
    ;(async () => {
      const response = await blogApi.addOrUpdate(formData)
      console.log('response: ', response)

      if (response.isSuccess) {
        alert('Đã lưu thành công!')
        const { from } = location.state || {}
        navigate(from?.pathname || '/admin', { replace: true })
      } else alert('Đã xảy ra lỗi!')
    })()
  }

  console.log('re-render')
  if (id && !isInteger(id)) {
    return <Navigate to={`/400?redirectTo=/admin/posts`} />
  }
  return (
    <>
      <Form method='post' encType='multipart/form-data' onSubmit={handleSubmit}>
        <Form.Control type='hidden' readOnly name='id' value={post.id} />
        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Tiêu đề</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              type='text'
              name='title'
              required
              value={post.title || ''}
              onChange={(e) => {
                setPost({ ...post, title: e.target.value })
              }}
            />
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Giới thiệu</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              as='textarea'
              type='text'
              name='shortDescription'
              title='Short Description'
              value={decode(post.shortDescription || '')}
              required
              onChange={(e) => {
                setPost({ ...post, shortDescription: e.target.value })
              }}
            />
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Nội dung</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              as='textarea'
              rows={10}
              type='text'
              name='description'
              title='Description'
              value={decode(post.description || '')}
              required
              onChange={(e) => {
                setPost({ ...post, description: e.target.value })
              }}
            />
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>MetaData</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              type='text'
              name='meta'
              title='Meta'
              value={decode(post.meta || '')}
              onChange={(e) => {
                setPost({ ...post, meta: e.target.value })
              }}
              required
            />
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Tác giả</Form.Label>
          <div className='col-sm-10'>
            <Form.Label className='visually-hidden'>Tác giả</Form.Label>
            <Form.Select
              name='authorId'
              title='Author Id'
              value={post.author.id}
              required
              onChange={(e) => {
                setPost({ ...post, author: e.target.value })
              }}
            >
              <option value=''>-- Chọn tác giả --</option>
              {filter.authorList.length > 0 &&
                filter.authorList.map((item, index) => (
                  <option key={index} value={item.value}>
                    {item.text}
                  </option>
                ))}
            </Form.Select>
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Chủ đề</Form.Label>
          <div className='col-sm-10'>
            <Form.Select
              title='CategoryId'
              name='categoryId'
              required
              value={post?.category?.id}
              onChange={(e) => {
                setPost({ ...post, category: e.target.value })
              }}
            >
              <option value=''>-- Chọn danh mục --</option>
              {filter.categoryList.length > 0 &&
                filter.categoryList.map((item, index) => (
                  <option key={index} value={item.value}>
                    {item.text}
                  </option>
                ))}
            </Form.Select>
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Từ khóa (mỗi từ 1 dòng)</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              as='textarea'
              rows={5}
              type='text'
              name='selectedTags'
              title='Selected Tags'
              value={post.selectedTags}
              onChange={(e) => {
                setPost({ ...post, selectedTags: e.target.value })
              }}
              required
            />
          </div>
        </div>

        {!isEmptyOrSpaces(post.imageUrl) && (
          <div className='row mb-3'>
            <Form.Label className='col-sm-2 col-form-labella'>Hình hiện tại</Form.Label>

            <div className='col-sm-10'>
              <img src={post.imageUrl} alt={post.title} />
            </div>
          </div>
        )}

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Chọn hình ảnh</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              type='file'
              name='imageFile'
              accept='image/*'
              title='Image file'
              onChange={(e) => {
                setPost({ ...post, imageFile: e.target.files[0] })
              }}
            />
          </div>
        </div>

        <div className='row mb-3'>
          <div className='col-sm-10 offset-sm-2'>
            <div className='form-check'>
              <input
                className='form-check-input'
                type='checkbox'
                name='published'
                checked={post.published}
                title='Published'
                onChange={(e) => setPost({ ...post, published: e.target.checked })}
              />
              <Form.Label className='form-check-label'> Đã xuất bản </Form.Label>
            </div>
          </div>
        </div>

        <div className='text-center'>
          <Link to='/admin/posts' className='btn btn-danger ms-2'>
            Huỷ và quay lại
          </Link>
          <Button variant='primary' type='submit'>
            Lưu các thay đỗi
          </Button>
        </div>
      </Form>
    </>
  )
}
export default Edit
