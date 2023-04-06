import React, { useEffect, useState } from 'react'
import { Card } from 'react-bootstrap'
import { Link, useParams } from 'react-router-dom'
import blogApi from '../api/blogApi'
import styles from '../styles/PostItem.module.css'
import { isEmptyOrSpaces } from '../utils/Utils'
import TagList from '../components/TagList'

PostDetail.propTypes = {}

function PostDetail(props) {
  const [post, setPost] = useState({})
  const { slug } = useParams()

  useEffect(() => {
    ;(async () => {
      try {
        const data = await blogApi.getDetailBySlug(slug)
        console.log('🚀 ~ ; ~ data:', data.result)
        if (data.isSuccess) {
          setPost(data.result)
        }
      } catch (error) {
        console.log(error)
      }
    })()
  }, [slug])

  let imageUrl = isEmptyOrSpaces(post.imageUrl)
    ? process.env.PUBLIC_URL + '/images/img1.jpg'
    : `${post.imageUrl}`

  return (
    <div>
      <Card.Img variant='top' className={styles.img} src={imageUrl} alt={post.title} />

      <Card.Body>
        <Card.Title>
          <Link to={`/blog/post/${post.urlSlug}`}>{post.title}</Link>
        </Card.Title>

        <Card.Text>
          <small className='text-text-muted'>Tác giả: </small>
          <span className='text-primary m-1'>{post?.author?.fullName}</span>

          <small className='text-text-muted'>Chủ đề: </small>
          <span className='text-primary m-1'>{post?.category?.name}</span>
        </Card.Text>

        <Card.Text>{post.shortDescription}</Card.Text>

        <div className='tag-list'>
          <TagList tagList={post.tags} />
        </div>
      </Card.Body>
    </div>
  )
}

export default PostDetail
