import Header from './Header';
import Footer from './Footer';
import PageList from './PageList';
import PageEdit from './PageEdit';
import Login from './Login';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <div className="App">
      <Header />
      <PageList />
      <PageEdit id="123" title="page title" url="page-url" content="page content" />
      <Login />
      <Footer />
    </div>
  );
}

export default App;
