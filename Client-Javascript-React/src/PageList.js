import { useState, useEffect } from 'react'
import Container from 'react-bootstrap/Container'
import Table from 'react-bootstrap/Table'
import Button from 'react-bootstrap/Button'
import ShowAlert from './ShowAlert'
import { api } from './Api'
import axios from "axios"

function PageList({component, setComponent, token}) {
  
  const id = undefined === component.requestData || undefined === component.requestData.id ? 'all' : component.requestData.id
  const [alert, setAlert] = useState({isShow: false, variant: "", heading: "", content: ""})
  const [pageList, setPageList] = useState([])

  const handleIsShow = (newIsShow) => {
    setAlert({isShow: newIsShow})
  }

  const handleEdit = (id) => {
    setComponent({componentName: 'PageEdit', requestData: {id: id}})
  }

  const handleDelete = (id) => {
    deletePage(id, token)
  }

  // const delay = (time) => {
  //   return new Promise(resolve => setTimeout(resolve, time));
  // }

  const getPage = (id, token) => {
    const url = api.page + "?id=" + id + "&token=" + token
    axios.get(url).then((response) => {
      if(response.data.type === "error") {
        setAlert({
          isShow: true,
          variant: "danger",
          heading: "Error",
          content: response.data.content
        })
      } else {
        setPageList(response.data.content)
        setAlert({
          isShow: false
        })
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

  const deletePage = (id, token) => {
    const url = api.page + "?id=" + id + "&token=" + token
    axios.delete(url)
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
        // delay(3000)
        getPage('all', token)
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
    getPage(id, token)
  }, [id, token])

  return (
    <div className="page-list">
      <Container>
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>ID</th>
              <th>Title</th>
              <th>Author</th>
              <th>Date</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {pageList.map((page) => (
            <tr key={page.id}>
              <td>{page.id}</td>
              <td>{page.title}</td>
              <td>{page.user_id}</td>
              <td>{page.date_published}</td>
              <td>
                <Button variant="link" onClick={() => { handleEdit(page.id) }} >Edit</Button>
                <Button variant="link" onClick={() => { handleDelete(page.id) }} >Delete</Button>
              </td>
            </tr>
            ))}
          </tbody>
        </Table>

        <ShowAlert isShow={alert.isShow} setIsShow={handleIsShow} variant={alert.variant} heading={alert.heading} content={alert.content} />

      </Container>
    </div>
  );

}

export default PageList