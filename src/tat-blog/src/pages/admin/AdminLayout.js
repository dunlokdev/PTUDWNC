import { Outlet } from 'react-router-dom'
import Navigation from '../../components/admin/Navigation'

const AdminLayout = () => {
  return (
    <>
      <Navigation />
      <div className='container-fluid py-3'>
        <Outlet />
      </div>
    </>
  )
}
export default AdminLayout
