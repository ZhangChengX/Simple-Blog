import { useState } from 'react'
import Header from './Header'
import Footer from './Footer'
import PageList from './PageList'
import PageEdit from './PageEdit'
import Login from './Login'
import 'bootstrap/dist/css/bootstrap.min.css'

export default function App() {

  const [user, setUser] = useState({isLogin: false, username: "", token: ""})
  const [component, setComponent] = useState({componentName: 'Login', requestData: {}, responseData: {}})

  return (
    <div className="App">
      <Header user={user} setUser={setUser} setComponent={setComponent} />
        {renderComponent(component, setComponent, user, setUser)}
      <Footer />
    </div>
  )
}

function renderComponent(component, setComponent, user, setUser) {
  switch(component.componentName) {
    case 'PageEdit':
      return <PageEdit component={component} setComponent={setComponent} token={user.token} />
    case 'PageList':
      return <PageList component={component} setComponent={setComponent} token={user.token} />
    default: // Login
      return <Login setComponent={setComponent} setUser={setUser} />
  }
}
