import { Button } from 'react-bootstrap'
import { Link } from 'react-router-dom'

export default function Pager({ postQuery, metadata }) {
  const { keyword } = postQuery || ''
  const { pageCount, hasNextPage, hasPreviousPage, pageNumber, pageSize } = metadata

  return (
    <>
      {pageCount > 1 ? (
        <div className='my-4 text-center'>
          {hasPreviousPage ? (
            <Link
              to={`/blog/?Keyword=${keyword}&PageNumber=${pageNumber - 1}&PageSize=${pageSize}`}
              className='btn btn-primary'
            >
              Trang trước
            </Link>
          ) : (
            <Button variant='outline-secondary' disabled>
              Trang trước
            </Button>
          )}
          {hasNextPage ? (
            <Link
              to={`/blog/?Keyword=${keyword}&PageNumber=${pageNumber + 1}&PageSize=${pageSize}`}
              className='btn btn-primary ms-1'
            >
              Trang sau
            </Link>
          ) : (
            <Button variant='outline-secondary' className='ms-1' disabled>
              Trang sau
            </Button>
          )}
        </div>
      ) : null}
    </>
  )
}
