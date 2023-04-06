import { useEffect, useState } from 'react'
import { ListGroup } from 'react-bootstrap'
import { Link } from 'react-router-dom'
import blogApi from '../../api/blogApi'

export default function CategoriesWidget() {
  const [postsFeature, setPostsFeature] = useState([])

  useEffect(() => {
    ;(async () => {
      try {
        const data = await blogApi.getRandom(5)
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
      <h3 className='mb-2 text-success'>Top 5 bài viết ngẫu nhiên</h3>
      {postsFeature.length > 0 && (
        <ListGroup>
          {postsFeature.map((post, index) => {
            let postedDate = new Date(post.postedDate)

            return (
              <ListGroup.Item key={index}>
                <Link to={`/blog/category?slug=${post.urlSlug}`} title={post.description}></Link>

                <Link
                  to={`/blog/post/${postedDate.getFullYear()}/${postedDate.getMonth()}/${postedDate.getDay()}/${
                    post.urlSlug
                  }`}
                >
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
