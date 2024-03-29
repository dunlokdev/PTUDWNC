import React from 'react'
import SearchForm from './SearchForm'
import CategoriesWidget from '../widgets/CategoriesWidget'
import PostWidget from '../widgets/PostWidget'
import PostRandomWidget from '../widgets/PostRandomWidget'

Sidebar.propTypes = {}

function Sidebar(props) {
  return (
    <div className='pt-4 ps-2'>
      <SearchForm />
      <CategoriesWidget />
      <PostWidget />
      <PostRandomWidget />
      <h1>Tìm kiếm bài viết</h1>
      <h1>Các chủ đề</h1>
      <h1>Bài viết nổi bật</h1>
      <h1>Đăng ký nhận tin mới</h1>
      <h1>Bài viết nổi bật</h1>
      <h1>Đăng ký nhận tin mới</h1>
      <h1>Tag Cloud</h1>
    </div>
  )
}

export default Sidebar
