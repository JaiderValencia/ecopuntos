import Header from './components/header'
import PrivateRoute from './components/routeProtection/privateRoutes'
import PublicRoute from './components/routeProtection/publicRoutes'
import PageNotFound from './pages/error/404'
import Home from './pages/home'
import Login from './pages/login'
import { Routes, Route } from 'react-router-dom'
import RegisterPage from './pages/register'
import ListEcopuntos from './pages/ecopuntos/ListEcopuntos'

function App() {
  return (
    <>
      <Header />
      <main className='min-h-screen pt-16 px-[4dvw]'>
        <Routes>

          //grupo de rutas públicas
          <Route element={<PublicRoute />}>
            <Route path='/login' element={<Login />} />
            <Route path='/registro' element={<RegisterPage />} />
          </Route>

          //grupo de rutas privadas
          <Route element={<PrivateRoute />}>
            <Route path='/' element={<Home />} />
            <Route path='/ecopuntos' element={<ListEcopuntos />} />
          </Route>

          <Route path='*' element={<PageNotFound />} />
        </Routes>
      </main>
    </>
  )
}

export default App