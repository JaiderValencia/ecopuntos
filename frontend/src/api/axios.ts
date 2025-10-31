import axios from 'axios'

const apiURL = 'http://localhost:5280/api'

const api = axios.create({
    baseURL: apiURL,
    headers: {
        'Content-Type': 'application/json',
    }
})

api.interceptors.request.use(config => {
    const { bearer } = JSON.parse(sessionStorage.getItem('userSession') || '{}') || ''

    if (bearer) {
        config.headers['Authorization'] = `Bearer ${bearer}`
    }
    return config
})

export default api