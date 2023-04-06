import TagList from './TagList'
import Card from 'react-bootstrap/Card'
import { Link } from 'react-router-dom'
import styles from '../styles/PostItem.module.css'
import { isEmptyOrSpaces } from '../utils/Utils'

const PostItem = ({ post }) => {
  let imageUrl = isEmptyOrSpaces(post.imageUrl)
    ? process.env.PUBLIC_URL + '/images/img1.jpg'
    : `${post.imageUrl}`

  let postedDate = new Date(post.postedDate)

  return (
    <article className='blog-entry mb-4'>
      <Card>
        <div className='row g-0'>
          <div className='col-md-3'>
            <Card.Img variant='top' className={styles.img} src={imageUrl} alt={post.title} />
          </div>

          <div className='col-md-9'>
            <Card.Body>
              <Card.Title>
                <Link to={`/blog/post/${post.urlSlug}`}>{post.title}</Link>
              </Card.Title>

              <Card.Text>
                <small className='text-text-muted'>Tác giả: </small>
                <Link to={`/blog/author/${post.author.urlSlug}`}>
                  <span className='text-primary m-1'>{post.author.fullName}</span>
                </Link>

                <small className='text-text-muted'>Chủ đề: </small>
                <Link to={`/blog/category/${post.category.urlSlug}`}>
                  <span className='text-primary m-1'>{post.category.name}</span>
                </Link>
              </Card.Text>

              <Card.Text>{post.shortDescription}</Card.Text>

              <div className='tag-list'>
                <TagList tagList={post.tags} />
              </div>

              <div className='text-end'>
                <Link
                  to={`/blog/post/${post.urlSlug}`}
                  className='btn btn-primary'
                  title={post.title}
                >
                  Xem chi tiết
                </Link>
              </div>
            </Card.Body>
          </div>
        </div>
      </Card>
    </article>
  )
}

export default PostItem
