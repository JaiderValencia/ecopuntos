import Header from './components/header'
import PageNotFound from './pages/error/404'
import Home from './pages/home'
import Login from './pages/login'
import { Routes, Route } from 'react-router-dom'

function App() {
  return (
    <>
      <Header />
      <main className='flex items-center justify-center min-h-screen pt-16'>
        <Routes>
          <Route path='/login' element={<Login />} />

          <Route path='/' element={<Home />} />
          <Route path='*' element={<PageNotFound />} />
        </Routes>
      </main>
    </>
  )
}

export default App