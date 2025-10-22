import axios from 'axios'

const apiURL = 'http://localhost:5280/api'

const api = axios.create({
    baseURL: apiURL,    
    headers: {
        'Content-Type': 'application/json'
    }
})

export default api