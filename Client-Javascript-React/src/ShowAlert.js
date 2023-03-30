import Alert from 'react-bootstrap/Alert';

export default function ShowAlert({isShow, setIsShow, variant="info", heading="info", content=""}) {
  return (
    <Alert variant={variant} show={isShow} onClose={() => setIsShow(false)} dismissible>
      <Alert.Heading>{heading}</Alert.Heading>
      {content}
    </Alert>
  )
}
