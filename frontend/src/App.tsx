import Header from './components/header'
import Login from './pages/login'

function App() {
  return (
    <>
      <Header />
      <main className='flex items-center justify-center min-h-screen pt-16'>
        <Login />
      </main>
    </>
  )
}

export default App