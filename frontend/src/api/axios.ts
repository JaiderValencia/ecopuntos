import axios from 'axios'

const apiURL = 'http://localhost:5280/api'

const { bearer } = JSON.parse(sessionStorage.getItem('userSession') || '{}') || ''

const api = axios.create({
    baseURL: apiURL,
    headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${bearer}`
    }
})

export default api