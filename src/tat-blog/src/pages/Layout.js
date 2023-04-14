import React from 'react'
import { Outlet } from 'react-router-dom'
import Navigation from '../components/blog/Navigation'
import SideBar from '../components/blog/Sidebar'

Layout.propTypes = {}

function Layout(props) {
  return (
    <>
      <Navigation />
      <div className='container-fluid py-3'>
        <div className='row'>
          <div className='col-9'>
            <Outlet />
          </div>

          <div className='col-3 border-start'>
            <SideBar />
          </div>
        </div>
      </div>
    </>
  )
}

export default Layout
