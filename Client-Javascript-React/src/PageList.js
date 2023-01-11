import React from 'react';
import Container from 'react-bootstrap/Container';
import Table from 'react-bootstrap/Table';
import Button from 'react-bootstrap/Button';

class PageList extends React.Component {
  render() {
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
              <tr>
                <td>1</td>
                <td>Title 1</td>
                <td>Author 1</td>
                <td>2000-01-12 12:00</td>
                <td>
                  <Button variant="link">Edit</Button>
                  <Button variant="link">Delete</Button>
                </td>
              </tr>
              <tr>
                <td>2</td>
                <td>Title 2</td>
                <td>Author 2</td>
                <td>2000-01-12 12:00</td>
                <td>
                  <Button variant="link">Edit</Button>
                  <Button variant="link">Delete</Button>
                </td>
              </tr>
            </tbody>
          </Table>
        </Container>
      </div>
    );
  }
}

export default PageList