import React from 'react';
import Alert from 'react-bootstrap/Alert';

class ShowAlert extends React.Component {
  constructor(props) {
    super(props);
    this.state = {isShowAlert: this.props.isShowAlert};
  }

  render() {
    if (this.state.isShowAlert) {
      return (
        <Alert variant={this.props.variant} onClose={() => this.setState({isShowAlert: false})} dismissible>
          <Alert.Heading>{this.props.heading}</Alert.Heading>
          {this.props.content}
        </Alert>
      );
    }
  }
}

export default ShowAlert;