import React from 'react'
import { Link } from 'react-router-dom'
import TagList from '../components/TagList'
import styles from '../styles/PostItem.module.css'
import { isEmptyOrSpaces } from '../utils/Utils'

const PostItem = ({ post }) => {
  const { urlSlug, title, shortDescription, imageUrl, category, author, tags } = post

  // let img = isEmptyOrSpaces(post.imageUrl)
  //   ? process.env.PUBLIC_URL + '/images/tips.jpg'
  //   : `${imageUrl}`

  return (
    <div className={`card ${styles.card}`}>
      <img src={'/images/tips.jpg'} className={`card-img-top ${styles.img}`} alt={title} />
      <div className='card-body'>
        <Link className='text-decoration-none' to={`/blog/post/${urlSlug}`} title='Read details'>
          <h5 className={`card-title ${styles.clamp}`}>{title}</h5>
        </Link>
        <TagList tags={tags} />
        <p className='text-muted mb-0'>
          Category:
          <Link className='text-decoration-none' to={`/blog/category/${category.urlSlug}`}>
            {category.name}
          </Link>
        </p>
        <p className='text-muted'>
          Author:
          <Link className='text-decoration-none' to={`/blog/author/${author.urlSlug}`}>
            {author.fullName}
          </Link>
        </p>
        <p className={`card-text ${styles.clamp}`} >{shortDescription}</p>
      </div>
    </div>
  )
}

export default PostItem
