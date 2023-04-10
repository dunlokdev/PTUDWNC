import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import About from './pages/About'
import Contact from './pages/Contact'
import Index from './pages/Index'
import Layout from './pages/Layout'
import PostDetail from './pages/PostDetail'
import Rss from './pages/Rss'
import AdminLayout from './pages/admin/AdminLayout'

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
          <Route path='/admin' element={<AdminLayout />} />
        </Routes>
      </BrowserRouter>
    </div>
  )
}

export default App
