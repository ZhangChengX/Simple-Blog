import { useState, useEffect } from 'react'
import Container from 'react-bootstrap/Container';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import ShowAlert from './ShowAlert'
import { api } from './Api'
import axios from "axios"

function PageEdit({component, setComponent, token}) {

  const id = undefined === component.requestData || undefined === component.requestData.id ? 'new' : component.requestData.id
  const [title, setTitle] = useState('')
  const [url, setUrl] = useState('')
  const [content, setContent] = useState('')
  const [userId, setUserId] = useState(1)
  const [alert, setAlert] = useState({isShow: false, variant: "", heading: "", content: ""})

  const handleIsShow = (newIsShow) => {
    setAlert({isShow: newIsShow})
  }

  const handleTitleChange = (e) => {
    setTitle(e.target.value)
  }

  const handleUrlChange = (e) => {   
    setUrl(e.target.value)
  }

  const handleContentChange = (e) => {   
    setContent(e.target.value)
  }

  const handleSubmit = (e) => {
    e.preventDefault()
    if ('new' === id) {
      addPage()
    } else {
      updatePage()
    }
  }

  const getPage = (id, token) => {
    const apiUrl = api.page + "?id=" + id + "&token=" + token
    axios.get(apiUrl).then((response) => {
      if(response.data.type === "error") {
        setAlert({
          isShow: true,
          variant: "danger",
          heading: "Error",
          content: response.data.content
        })
      } else {
        // console.log(response.data.content)
        if(1 === response.data.content.length) {
          setTitle(response.data.content[0].title)
          setUrl(response.data.content[0].url)
          setContent(response.data.content[0].content)
          setUserId(response.data.content[0].user_id)
          setAlert({
            isShow: false
          })
        } else {
          setAlert({
            isShow: true,
            variant: "danger",
            heading: "Error",
            content: 'Incorrect id: ' + id
          })
        }
      }
    }).catch((error) => {
      setAlert({
        isShow: true,
        variant: "danger",
        heading: "Error",
        content: error.message
      })
    })
  }

  const updatePage = () => {
    const dateModified = Date.now()
    const apiUrl = api.page + "?id=" + id + "&token=" + token
    const config = {
      headers: {
        'Content-Type': 'application/json'
      }
    }
    const data = {
        url: url,
        title: title,
        content: content,
        user_id: userId,
        date_modified: dateModified
      }
    axios.put(apiUrl, data, config)
    .then(function (response) {
      if(response.data.type === "error") {
        setAlert({
          isShow: true,
          variant: "danger",
          heading: "Error",
          content: response.data.content
        })
      } else {
        setAlert({
          isShow: true,
          variant: "success",
          heading: "Success",
          content: response.data.content
        })
      }
    }).catch(function (error) {
      setAlert({
        isShow: true,
        variant: "danger",
        heading: "Error",
        content: error.message
      })
    })
  }

  const addPage = () => {
    const datePublished = Date.now()
    const apiUrl = api.page + "?token=" + token
    const config = {
      headers: {
        'Content-Type': 'application/json'
      }
    }
    const data = {
        url: url,
        title: title,
        content: content,
        user_id: userId,
        date_published: datePublished
      }
    axios.post(apiUrl, data, config)
    .then(function (response) {
      if(response.data.type === "error") {
        setAlert({
          isShow: true,
          variant: "danger",
          heading: "Error",
          content: response.data.content
        })
      } else {
        setAlert({
          isShow: true,
          variant: "success",
          heading: "Success",
          content: response.data.content
        })
      }
    }).catch(function (error) {
      setAlert({
        isShow: true,
        variant: "danger",
        heading: "Error",
        content: error.message
      })
    })
  }

  useEffect(() => {
    if('new' !== id) {
      getPage(id, token)
    }
  }, [id, token])

  return (
    <div className="page-edit">
      <Container>
        <Form>
          <Form.Group className="mb-3">
            <Form.Label>Title</Form.Label>
            <Form.Control type="text" placeholder="Enter Title" onInput={handleTitleChange} defaultValue={title} />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>URL</Form.Label>
            <Form.Control type="text" placeholder="Enter URL" onInput={handleUrlChange} defaultValue={url} />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Content</Form.Label>
            <Form.Control as="textarea" rows={3} onInput={handleContentChange} defaultValue={content} />
          </Form.Group>

          <Form.Control type="hidden" defaultValue={id} />
          <Form.Control type="hidden" defaultValue={userId} />

          <ShowAlert isShow={alert.isShow} setIsShow={handleIsShow} variant={alert.variant} heading={alert.heading} content={alert.content} />

          <Button variant="primary" type="submit" onClick={handleSubmit}>
            Submit
          </Button>
        </Form>
      </Container>
    </div>
  )
}

export default PageEdit