import axios from 'axios'

const apiURL = 'http://localhost:4000/api'

const api = axios.create({
    baseURL: apiURL,
    withCredentials: true,
})

export default api