import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';

function Header({user, setUser, setComponent}) {

  function handleLogout() {
    setUser({isLogin: false, username: "", token: ""})
    handleNav('Login')
  }

  function handleNewPage() {
    // handleNav('PageEdit')
    setComponent({componentName: 'PageEdit', requestData: {id: 'new'}})
  }

  function handleNav(componentName) {
    setComponent({componentName: componentName})
  }

  return (
    <header className="header mb-3">
      <Navbar bg="light" expand="lg">
        <Container>
          <Navbar.Brand href="#home">App</Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link href="#login" onClick={() => {handleNav('Login')}}>Login</Nav.Link>
              <Nav.Link href="#page" onClick={() => {handleNav('PageList')}}>Page</Nav.Link>
              <Nav.Link href="#new" onClick={() => {handleNewPage()}}>New</Nav.Link>
              {user.isLogin && 
              <>
                <Nav.Link href="#username">{user.username}</Nav.Link>
                <Nav.Link href="#logout" onClick={handleLogout}>Logout</Nav.Link>
              </>
              }
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </header>
  )
}

export default Header