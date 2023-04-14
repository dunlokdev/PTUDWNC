import React, { useEffect } from 'react'

About.propTypes = {}

function About(props) {
  useEffect(() => {
    document.title = 'Trang gioi thiệu'
  })

  return (
    <div>
      <h1>Đây là trang gioi thiệu</h1>
    </div>
  )
}

export default About
