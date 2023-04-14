import axiosClient from './axiosClient'

const blogApi = {
  getAll(params) {
    const url = '/posts'
    return axiosClient.get(url, { params })
  },
  getFilter() {
    const url = '/posts/get-filter'
    return axiosClient.get(url)
  },
  addOrUpdate(data) {
    const url = `/posts`
    return axiosClient.post(url, data, { headers: { 'Content-Type': 'multipart/form-data' } })
  },
  getById(id) {
    const url = `/posts/${id}`
    return axiosClient.get(url)
  },
  getFeature(limit) {
    const url = `/posts/featured/${limit}`
    return axiosClient.get(url)
  },
  getRandom(limit) {
    const url = `/posts/random/${limit}`
    return axiosClient.get(url)
  },
  getDetailBySlug(slug) {
    const url = `/posts/byslug/${slug}`
    return axiosClient.get(url)
  }
}

export default blogApi
