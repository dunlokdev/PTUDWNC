import axiosClient from './axiosClient'

const blogApi = {
  getAll(params) {
    const url = '/posts'
    return axiosClient.get(url, { params })
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
