import { useRef } from 'react'
import { useEffect } from 'react'
import { useState } from 'react'
import { Button, Form, Modal } from 'react-bootstrap'
import authorApi from '../../api/authorApi'
import categoryApi from '../../api/categoryApi'
import blogApi from '../../api/blogApi'

const ModalPost = ({ show, onShow, id }) => {
  const [authors, setAuthors] = useState([])
  const [categories, setCategories] = useState([])
  const [post, setPost] = useState({})

  useEffect(() => {
    ;(async () => {
      try {
        const post = await blogApi.getById(id)

        const authors = await authorApi.getAll()
        const categories = await categoryApi.getAll()

        setAuthors(authors.result)
        setPost(post.result)
        setCategories(categories.result)
      } catch (error) {
        console.log(error)
      }
    })()
  }, [id])

  const idRef = useRef()
  const titleRef = useRef()
  const shortDescriptionRef = useRef()
  const descriptionRef = useRef()
  const metaRef = useRef()
  const urlSlugRef = useRef()
  const tagRef = useRef()
  const authorRef = useRef()
  const categoryRef = useRef()

  const handleClose = () => {
    onShow(false)
  }

  const handleEditSubmit = (e) => {
    e.preventDefault()

    console.log(titleRef.current.value)
    console.log(shortDescriptionRef.current.value)
    console.log(descriptionRef.current.value)
    console.log(metaRef.current.value)
    console.log(urlSlugRef.current.value)
    console.log(tagRef.current.value)
    console.log(categoryRef.current.value)
    console.log(authorRef.current.value)
  }

  return (
    <Modal show={show} onHide={handleClose}>
      <form onSubmit={handleEditSubmit}>
        <Modal.Header closeButton>
          <Modal.Title>Thêm/Sửa</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form.Control type='hidden' readOnly name='id' ref={idRef} defaultValue={post.id} />
          <div className='row mb-3'>
            <Form.Label className='col-sm-2 col-form-label'>Tiêu đề</Form.Label>
            <div className='col-sm-10'>
              <Form.Control
                type='text'
                name='title'
                ref={titleRef}
                defaultValue={post.title}
                required
              />
            </div>
          </div>

          <div className='row mb-3'>
            <Form.Label className='col-sm-2 col-form-label'>Url slug</Form.Label>
            <div className='col-sm-10'>
              <Form.Control
                type='text'
                name='urlSlug'
                ref={urlSlugRef}
                defaultValue={post.urlSlug}
                required
              />
            </div>
          </div>

          <div className='row mb-3'>
            <Form.Label className='col-sm-2 col-form-label'>Giới thiệu</Form.Label>
            <div className='col-sm-10'>
              <Form.Control
                as='textarea'
                type='text'
                name='title'
                ref={shortDescriptionRef}
                defaultValue={post.shortDescription}
                required
              />
            </div>
          </div>

          <div className='row mb-3'>
            <Form.Label className='col-sm-2 col-form-label'>Nội dung</Form.Label>
            <div className='col-sm-10'>
              <Form.Control
                as='textarea'
                type='text'
                name='title'
                ref={descriptionRef}
                defaultValue={post.description}
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
                ref={metaRef}
                defaultValue={post.meta}
                required
              />
            </div>
          </div>

          <div className='row mb-3'>
            <Form.Label className='col-sm-2 col-form-label'>Tác giả</Form.Label>
            <div className='col-sm-10'>
              <Form.Label className='visually-hidden'>Tác giả</Form.Label>
              <Form.Select ref={authorRef} title='Tác giả' name='authorId'>
                <option value=''>-- Chọn tác giả --</option>
                {authors.map((author) => (
                  <option key={author.id} value={author.id}>
                    {author.fullName}
                  </option>
                ))}
              </Form.Select>
            </div>
          </div>

          <div className='row mb-3'>
            <Form.Label className='col-sm-2 col-form-label'>Danh mục</Form.Label>
            <div className='col-sm-10'>
              <Form.Label className='visually-hidden'>Danh mục</Form.Label>
              <Form.Select ref={categoryRef} title='Tác giả' name='authorId'>
                <option value=''>-- Chọn danh mục --</option>
                {categories.map((category) => (
                  <option key={category.id} value={category.id}>
                    {category.name}
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
                ref={tagRef}
                defaultValue=''
                required
              />
            </div>
          </div>
        </Modal.Body>
        <Modal.Footer>
          <Button variant='secondary' onClick={handleClose}>
            Close
          </Button>
          <Button variant='primary' type='submit'>
            Save Changes
          </Button>
        </Modal.Footer>
      </form>
    </Modal>
  )
}
export default ModalPost
