import { useEffect, useState } from 'react'
import { Table } from 'react-bootstrap'
import blogApi from '../../../api/blogApi'
import Loading from '../../../components/Loading'
import PostFilterPane from '../../../components/admin/PostFilterPane'
import { Link, useLocation } from 'react-router-dom'

const Posts = () => {
  const [posts, setPosts] = useState([])
  const [loading, setLoading] = useState(true)
  const location = useLocation()

  const [filters, setFilters] = useState({
    Keyword: '',
    PageSize: 10,
    PageNumber: 1
  })

  useEffect(() => {
    document.title = 'Danh sách bài viết'
    ;(async () => {
      try {
        const data = await blogApi.getAll(filters)
        if (data.isSuccess) {
          setPosts(data.result.items)
        }
      } catch (error) {
        console.log(error)
      }
      setLoading(false)
    })()
  }, [filters])

  const handeFilterChange = (value) => {
    const newFilters = { ...filters, ...value }
    setFilters(newFilters)
  }

  return (
    <>
      <PostFilterPane postQuery={filters} onChange={handeFilterChange} />
      {loading ? (
        <Loading />
      ) : (
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th>Tiêu đề</th>
              <th>Tác giả</th>
              <th>Chủ đề</th>
              <th>Xuất bản</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {posts.length > 0 ? (
              posts.map((post) => (
                <tr key={post.id}>
                  <td>
                    <p className='text-bold fw-bold text-primary mb-0 poi'>{post.title}</p>
                    <p className='text-muted'>{post.shortDescription}</p>
                  </td>
                  <td>{post.author.fullName}</td>
                  <td>{post.category.name}</td>
                  <td>{post.published ? 'Có' : 'Không'}</td>
                  <td>
                    <div className='d-flex gap-1'>
                      <Link
                        to={`${location.pathname}/posts/edit/${post.id}`}
                        className='btn btn-warning btn-sm'
                      >
                        Sửa
                      </Link>
                      <button className='btn btn-danger btn-sm'>Xoá</button>
                    </div>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={4}>
                  <h4 className='text-center text-danger'>Không tìm thấy bài viết</h4>
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      )}
    </>
  )
}
export default Posts
