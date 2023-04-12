import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import About from './pages/About'
import Contact from './pages/Contact'
import Index from './pages/Index'
import Layout from './pages/Layout'
import PostDetail from './pages/PostDetail'
import Rss from './pages/Rss'
import AdminLayout from './pages/admin/AdminLayout'
import NotFound from './pages/NotFound'
import Badrequest from './pages/Badrequest'
import Posts from './pages/admin/post/Posts'

function App() {
  return (
    <div>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<Layout />}>
            <Route path='/' element={<Index />} />
            <Route path='blog' element={<Index />} />
            <Route path='blog/post/:slug' element={<PostDetail />} />
            <Route path='blog/contact' element={<Contact />} />
            <Route path='blog/About' element={<About />} />
            <Route path='blog/Rss' element={<Rss />} />
          </Route>
          <Route path='/admin' element={<AdminLayout />}>
            <Route index element={<Posts />} />
          </Route>
          <Route path='/400' element={<Badrequest />} />
          <Route path='*' element={<NotFound />} />
        </Routes>
      </BrowserRouter>
    </div>
  )
}

export default App
