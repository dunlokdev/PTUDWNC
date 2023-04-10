import { useEffect, useState } from 'react'
import { ListGroup } from 'react-bootstrap'
import { Link } from 'react-router-dom'
import blogApi from '../../api/blogApi'

export default function CategoriesWidget() {
  const [postsFeature, setPostsFeature] = useState([])

  useEffect(() => {
    ;(async () => {
      try {
        const data = await blogApi.getFeature(3)
        if (data.isSuccess) {
          setPostsFeature(data.result)
        }
      } catch (error) {
        console.log(error)
      }
    })()
  }, [])

  return (
    <div className='mb-4'>
      <h3 className='mb-2 text-success'>Top 3 bài viết nổi bật</h3>
      {postsFeature.length > 0 && (
        <ListGroup>
          {postsFeature.map((post, index) => {
            return (
              <ListGroup.Item key={index}>
                <Link to={`/blog/post/${post.urlSlug}`} title={post.description}>
                  {post.title}
                  <span>&nbsp;({post.viewCount})</span>
                </Link>
              </ListGroup.Item>
            )
          })}
        </ListGroup>
      )}
    </div>
  )
}
