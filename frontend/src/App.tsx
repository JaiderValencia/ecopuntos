import Header from './components/header'
import PrivateRoute from './components/routeProtection/privateRoutes'
import PublicRoute from './components/routeProtection/publicRoutes'
import PageNotFound from './pages/error/404'
import Home from './pages/home'
import Login from './pages/login'
import { Routes, Route } from 'react-router-dom'

function App() {
  return (
    <>
      <Header />
      <main className='min-h-screen pt-16'>
        <Routes>

          //grupo de rutas públicas
          <Route element={<PublicRoute />}>
            <Route path='/login' element={<Login />} />
          </Route>

          //grupo de rutas privadas
          <Route element={<PrivateRoute />}>
            <Route path='/' element={<Home />} />
          </Route>

          <Route path='*' element={<PageNotFound />} />
        </Routes>
      </main>
    </>
  )
}

export default App