import React from 'react'
import Container from 'react-bootstrap/Container'
import Button from 'react-bootstrap/Button'
import Form from 'react-bootstrap/Form'
import { api } from './Api'
import ShowAlert from './ShowAlert'
import axios from "axios"

class Login extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      username: "",
      password: "",
      isShow: false,
      variant: "success",
      heading: "Success",
      content: ""
    }
  }

  handleTextChange = (e, key) => {   
    this.setState({[key]: e.target.value})
  }

  // Allow child component <ShowAlert> change parent state
  handleIsShow = (newIsShow) => {
    this.setState({isShow: newIsShow})
  }

  handleSubmit = (e) => {
    e.preventDefault()
    if (this.state.username === "" && this.state.password === "") {
      this.setState({
        isShow: true,
        variant: "danger",
        heading: "Error",
        content: "Please enter username and password"
      })
    } else {
      // Send login request
      const url = api.userLogin
      // `params` are the URL parameters to be sent with the request
      const params = {
        "username": this.state.username,
        "password": this.state.password
      }
      // `data` is the data to be sent as the request body
      // const data = {}
      axios.get(url, { 
        headers: {'Content-Type': 'application/json'},
        params: params 
      }).then((response) => {
        if(response.data.type === "error") {
          this.setState({
            isShow: true,
            variant: "danger",
            heading: "Error",
            content: response.data.content
          })
        } else {
          this.props.setUser({
            isLogin: true, username: params.username, token: response.data.content
          })
          this.props.setComponent({
            componentName: 'PageList', requestData: {id: 'all'}
          })
          this.setState({
            isShow: false
          })
        }
      }).catch((error) => {
        this.setState({
          isShow: true,
          variant: "danger",
          heading: "Error",
          content: error.message
        })
      })
    }
  }

  render() {
    return (
      <Container>
        <Form>
          <Form.Group className="mb-3" controlId="formUsername">
            <Form.Label>Username</Form.Label>
            <Form.Control type="text" placeholder="Enter username" onInput={(e) => this.handleTextChange(e, "username")} />
          </Form.Group>

          <Form.Group className="mb-3" controlId="formPassword">
            <Form.Label>Password</Form.Label>
            <Form.Control type="password" placeholder="Password" onInput={(e) => this.handleTextChange(e, "password")} />
          </Form.Group>

          <ShowAlert isShow={this.state.isShow} setIsShow={this.handleIsShow} variant={this.state.variant} heading={this.state.heading} content={this.state.content} />

          <Button variant="primary" type="submit" onClick={this.handleSubmit}>
            Submit
          </Button>
        </Form>
      </Container>
    )
  }
}

export default Login