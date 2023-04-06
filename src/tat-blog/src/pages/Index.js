import { useState, useEffect, useMemo } from 'react'
import { useLocation } from 'react-router-dom'
import blogApi from '../api/blogApi'
import styles from '../styles/Index.module.css'
import PostItem from '../components/PostItem'
import Pager from '../components/Pager'

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
  const pageSize = query.get('PageSize') ?? 5
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

      setTimeout(() => {
        setLoading(false)
      }, 2000)
    })()
  }, [keyword, pageSize, pageNumber])

  useEffect(() => {
    window.scrollY = 0
  }, [postList])

  return (
    <>
      {/* {postList.length > 0 ? (
        <div className='p-4'>
          {postList.map((post) => (
            <PostItem key={post.id} post={post} />
          ))}
          <Pager postQuery={{ keyword }} metadata={metadata} />
        </div>
      ) : (
        <h1 className='text-danger'>Loading</h1>
      )} */}

      {loading ? (
        <div className={styles.loaderContainer}>
          <div className={styles.spinner}></div>
        </div>
      ) : (
        <div className='p-4'>
          {postList.map((post) => (
            <PostItem key={post.id} post={post} />
          ))}
          <Pager postQuery={{ keyword }} metadata={metadata} />
        </div>
      )}
    </>
  )
}
