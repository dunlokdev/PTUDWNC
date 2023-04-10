import { useEffect, useMemo, useState } from 'react'
import { useLocation } from 'react-router-dom'
import blogApi from '../api/blogApi'
import Pager from '../components/Pager'
import PostGrid from '../components/PostGrid'
import styles from '../styles/Index.module.css'

export default function Home() {
  const [postList, setPostList] = useState([])
  const [metadata, setMetadata] = useState([])
  const [loading, setLoading] = useState(true)

  const useQuery = () => {
    const { search } = useLocation()
    return useMemo(() => new URLSearchParams(search), [search])
  }

  const query = useQuery()
  const keyword = query.get('Keyword') ?? ''
  const pageSize = query.get('PageSize') ?? 6
  const pageNumber = query.get('PageNumber') ?? 1

  useEffect(() => {
    document.title = 'Trang chủ'
    ;(async () => {
      try {
        const data = await blogApi.getAll({ keyword, pageSize, pageNumber })
        if (data.isSuccess) {
          setPostList(data.result.items)
          setMetadata(data.result.metadata)
        }
      } catch (error) {
        console.log(error)
      }

      setLoading(false)
    })()
  }, [keyword, pageSize, pageNumber])

  useEffect(() => {
    window.scrollY = 0
  }, [postList])

  return (
    <>
      {loading ? (
        <div className={styles.loaderContainer}>
          <div className={styles.spinner}></div>
        </div>
      ) : (
        <div className='mb-5 mt-3'>
          <h3 className='text-center text-primary mb-4'>Danh sách các bài viết</h3>

          <PostGrid columns={3} articles={postList} />
          <Pager postQuery={{ keyword }} metadata={metadata} />
        </div>
      )}
    </>
  )
}
