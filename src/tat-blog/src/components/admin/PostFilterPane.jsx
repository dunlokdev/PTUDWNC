import { useEffect, useRef, useState } from 'react'
import { Button, Form } from 'react-bootstrap'
import { Link } from 'react-router-dom'
import authorApi from '../../api/authorApi'
import categoryApi from '../../api/categoryApi'

const PostFilterPane = ({ postQuery, onChange }) => {
  const MONTHS = Array.from({ length: 12 }, (_, i) => i + 1)
  //=> [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

  const keywordRef = useRef()
  const authorRef = useRef()
  const categoryRef = useRef()
  const yearRef = useRef()
  const monthRef = useRef()

  const [authors, setAuthors] = useState([])
  const [categories, setCategories] = useState([])

  useEffect(() => {
    ;(async () => {
      const authors = await authorApi.getAll()
      const categories = await categoryApi.getAll()
      setAuthors(authors.result)
      setCategories(categories.result)
    })()
  }, [])

  const handleFilterPosts = (e) => {
    e.preventDefault()

    const newFilter = {
      ...postQuery,
      Keyword: keywordRef.current.value,
      AuthorId: authorRef.current.value || null,
      CategoryId: categoryRef.current.value || null,
      Year: yearRef.current.value || null,
      Month: monthRef.current.value || null
    }
    if (onChange) {
      onChange(newFilter)
    }
  }
  const handleClearFilter = () => {
    keywordRef.current.value = ''
    authorRef.current.value = ''
    categoryRef.current.value = ''
    yearRef.current.value = ''
    monthRef.current.value = ''
    const newFilter = {
      ...postQuery,
      Keyword: keywordRef.current.value,
      AuthorId: authorRef.current.value || null,
      CategoryId: categoryRef.current.value || null,
      Year: yearRef.current.value || null,
      Month: monthRef.current.value || null
    }
    if (onChange) {
      onChange(newFilter)
    }
  }

  return (
    <Form
      method='get'
      onSubmit={handleFilterPosts}
      className='row gx-3 gy-2 align-items-center py-2'
    >
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>Từ khóa</Form.Label>
        <Form.Control ref={keywordRef} type='text' placeholder='Nhập từ khóa...' name='keyword' />
      </Form.Group>
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>Tác giả</Form.Label>
        <Form.Select ref={authorRef} title='Tác giả' name='authorId'>
          <option value=''>-- Chọn tác giả --</option>
          {authors.map((author) => (
            <option key={author.id} value={author.id}>
              {author.fullName}
            </option>
          ))}
        </Form.Select>
      </Form.Group>
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>Chủ đề</Form.Label>
        <Form.Select ref={categoryRef} title='Chủ đề' name='categoryId'>
          <option value=''>-- Chọn chủ đề --</option>
          {categories.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </Form.Select>
      </Form.Group>
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>Nhập năm</Form.Label>
        <Form.Control ref={yearRef} type='text' placeholder='Nhập năm...' name='year' />
      </Form.Group>
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>Tháng</Form.Label>
        <Form.Select ref={monthRef} title='Tháng' name='month'>
          <option value=''>-- Chọn tháng --</option>
          {MONTHS.map((month) => (
            <option key={month} value={month}>
              Tháng {month}
            </option>
          ))}
        </Form.Select>
      </Form.Group>
      <Form.Group className='col-auto'>
        <Button variant='primary' type='submit'>
          Tìm/Lọc
        </Button>
        <Button variant='warning mx-2' onClick={handleClearFilter}>
          Bỏ lọc
        </Button>
        <Link to='/admin/posts/edit' className='btn btn-success'>
          Thêm mới
        </Link>
      </Form.Group>
    </Form>
  )
}
export default PostFilterPane
