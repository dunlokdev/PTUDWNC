import { Button, Form } from "react-bootstrap"
import { useParams } from "react-router-dom"

const Edit = () => {
  // state
  const [post, setPost] = useState({
    id: 0,
    title: '',
    shortDescription: '',
    description: '',
    meta: '',
    urlSlug: '',
    imageUrl: '',
    category: {},
    author: {},
    tags: [],
    selectedTags: '',
    published: true,
  })

  const [filter, setFilter] = useState({
    authorList: [],
    categoryList: []
  })

  // get from params
  const {id} = useParams();

  // Call API
  useEffect(() => {
    document.title = 'Thêm/Cập nhật bài viết';

    ;(async () => {
      try {
        const post = await blogApi.getById(id)
        console.log(">>> Check post: ", post);

      } catch (error) {
        console.log(error)
      }
    })()
  }, [id])

  // Method handle event
  const handleSubmit = (e) => {

  }


  return (
    <>
      <Form
        method="post"
        encType='multipart/form-data'
        onSubmit={handleSubmit}
      >

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
                setPost({...post, title: e.target.value})
              }}
            />
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Url slug</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              type='text'
              name='urlSlug'
              title="Url slug"
              value={post.urlSlug || ''}
              required
              onChange={(e) => {
                setPost({...post, urlSlug: e.target.value})
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
              title="Short Description"
              value={decode(post.shortDescription || '')}
              required
              onChange={(e) => {setPost({...post, shortDescription: e.target.value})}}
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
            />
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Meta</Form.Label>
          <div className='col-sm-10'>
            <Form.Control
              type='text'
              name='meta'
              title="Meta"
              value={decode(post.meta || '')}
              onChange={e => {setPost({...post, meta: e.target.value})}}
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
              value={post?.author?.id}
              required
              onChange={e => {setPost({...post, author: e.target.value})}}
            >
              <option value=''>-- Chọn tác giả --</option>
              {
                filter.authorList.length > 0
                   && filter.authorList.map((item, index) =>
                     <option key={item.id} value={item.value}>{item.text}</option>)
              }
            </Form.Select>
          </div>
        </div>

        <div className='row mb-3'>
          <Form.Label className='col-sm-2 col-form-label'>Danh mục</Form.Label>
          <div className='col-sm-10'>
            <Form.Select title='CategoryId' name='categoryId' required value={post.category.id} onChange={e => {setPost({...post, category: e.target.value})}}>
              <option value=''>-- Chọn danh mục --</option>
              {
                filter.categoryList.length > 0
                   && filter.categoryList.map((item, index) =>
                     <option key={item.id} value={item.value}>{item.text}</option>)
              }
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
              title= 'Selected Tags'
              value={post.selectedTags}
              required
            />
          </div>
        </div>

        <Button variant='secondary' onClick={}>
          Close
        </Button>
        <Button variant='primary' type='submit'>
          Save Changes
        </Button>
    </Form>
    </>
  )
}
export default Edit