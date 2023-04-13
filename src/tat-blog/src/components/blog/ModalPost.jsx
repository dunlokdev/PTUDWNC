import { useRef } from 'react'
import { useEffect } from 'react'
import { useState } from 'react'
import { Button, Form, Modal } from 'react-bootstrap'
import authorApi from '../../api/authorApi'
import categoryApi from '../../api/categoryApi'
import blogApi from '../../api/blogApi'

const ModalPost = ({ show, onShow, id, handleSubmit }) => {
  /*
  {
    "id": 1,
    "title": "Cách dễ nhất để học HTML CSS cho người mới bắt đầu!",
    "shortDescription": "Thực hành 8 dự án trên Figma, 300+ bài tập và thử thách, mua một lần học mãi mãi, được thiết kế và hướng dẫn bởi Sơn Đặng.",
    "description": "Thực hành 8 dự án với thiết kế trên Figma\r\nFigma là công cụ thiết kế UI/UX hàng đầu thế giới hiện nay. Với 8 dự án thực hành trên Figma, bạn có thể tự làm lại hầu hết mọi giao diện trang web mà bạn thấy.\r\n\r\nKhóa học được thiết kế bởi Sơn Đặng\r\nSơn Đặng là CEO - Founder của Cộng Đồng Học Lập Trình F8. Hiện tại, anh vẫn là một Fullstack developer với hơn 8 năm kinh nghiệm làm việc thực tế.",
    "meta": "post-01",
    "urlSlug": "cach-de-nhat-de-hoc-html-css-cho-nguoi-moi-bat-dau",
    "imageUrl": "uploads/pictures/d32727c7856b4be780bfa1d1fde5daaf.png",
    "viewCount": 5,
    "postedDate": "2023-02-22T01:20:00",
    "modifiedDate": "2023-03-25T18:52:16.087",
    "category": {
      "id": 1,
      "name": ".NET Core",
      "urlSlug": "net-core"
    },
    "author": {
      "id": 8,
      "fullName": "Dương Mỹ Lộc",
      "urlSlug": "duong-my-loc"
    },
    "tags": [
      {
        "id": 24,
        "name": "HTML",
        "urlSlug": "html"
      },
      {
        "id": 25,
        "name": "CSS",
        "urlSlug": "css"
      },
      {
        "id": 26,
        "name": "PRO",
        "urlSlug": "pro"
      }
    ]
  },
  */
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
    published: true
  })

  const [filter, setFilter] = useState({
    authorList: [],
    categoryList: []
  })

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

  const getTags = () => {
    const list = []
    let tags = ''
    post?.tags?.forEach((element) => {
      list.push(element.name)
      tags = list.join()
    })

    return tags
  }

  const handleClose = () => {
    onShow(false)
  }

  const handleEditSubmit = (e) => {
    e.preventDefault()
    if (!handleSubmit) return

    const newPost = {
      ...post
    }
    // handleSubmit(newPost)
  }

  return (
    <Modal show={show} onHide={handleClose}>
      <form onSubmit={handleEditSubmit}>
        <Modal.Header closeButton>
          <Modal.Title>Thêm/Sửa</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form.Control type='hidden' readOnly name='id' value={post.id} />
          <div className='row mb-3'>
            <Form.Label className='col-sm-2 col-form-label'>Tiêu đề</Form.Label>
            <div className='col-sm-10'>
              <Form.Control
                type='text'
                name='title'
                value={post.title}
                onChange={(e) => {
                  setPost(...post, e.target.value)
                }}
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
              <Form.Select
                ref={authorRef}
                title='Tác giả'
                name='authorId'
                defaultValue={post?.author?.id}
              >
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
                defaultValue={getTags()}
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
