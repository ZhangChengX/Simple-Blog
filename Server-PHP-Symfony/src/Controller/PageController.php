<?php
namespace App\Controller;

use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Doctrine\ORM\EntityManagerInterface;
use App\Entity\Page;
use App\Entity\User;

class PageController extends AbstractController {

    #[Route('/page',  methods: ['GET'])]
    public function page_list(EntityManagerInterface $entityManager): Response {
        $pages = $entityManager->getRepository(Page::class)->findAll();
        $data = [];
        $page_list = [];
        foreach($pages as $page) {
            $page_list[] = array(
                'id' => $page->getId(),
                'user_id' => $page->getUserId(),
                'url' => $page->getUrl(),
                'title' => $page->getTitle(),
                'content' => $page->getContent(),
                'date_published' => date("Y-m-d H:i", $page->getDatePublished()),
                'date_modified' => date("Y-m-d H:i", $page->getDateModified())
            );
        }
        $data['title'] = 'Page List';
        $data['page_list'] = $page_list;
        return $this->render('page_list.html.twig', $data);
    }

    #[Route('/page/{id}',  methods: ['GET'])]
    public function page_detail(string $id, EntityManagerInterface $entityManager): Response {
        $pageRepository = $entityManager->getRepository(Page::class);
        $userRepository = $entityManager->getRepository(User::class);
        if(is_numeric($id)) {
            $page = $pageRepository->find($id);
            $idType = 'id';
        } else {
            $page = $pageRepository->findOneBy(['url' => $id]);
            $idType = 'url';
        }
        if (!$page) {
            return new Response(
                '<html><body> No page found for ' . $idType . ': ' . $id . ' </body></html>'
            );
        }
        if($page->getDateModified() != 0) {
            $date = $page->getDateModified();
        } else {
            $date = $page->getDatePublished();
        }
        $user = $userRepository->find($page->getUserId());
        if(!$user) {
            $username = 'Unknown';
        } else {
            $username = $user->getUsername();
        }
        $data = [
            'id' => $page->getId(),
            'username' => $username,
            'url' => $page->getUrl(),
            'title' => $page->getTitle(),
            'content' => $page->getContent(),
            'date' => date("Y-m-d H:i", $date)
        ];
        return $this->render('page_detail.html.twig', $data);
    }

    #[Route('/api/page',  methods: ['GET'])]
    public function get_page(Request $request, EntityManagerInterface $entityManager): JsonResponse {
        $data = array('type' => 'error', 'content' => '');
        if ('GET' != $request->getMethod()) {
            $data['content'] = 'Bad request.';
            return $this->json($data);
        }
        $pageRepository = $entityManager->getRepository(Page::class);
        $id = $request->query->get('id');
        $page = $pageRepository->find($id);
        if (!$page) {
            $data['content'] = 'No page found for id: ' . $id;
            return $this->json($data);
        }
        $data['content'] = [
            'id' => $page->getId(),
            'username' => $page->getUserId(),
            'url' => $page->getUrl(),
            'title' => $page->getTitle(),
            'content' => $page->getContent(),
            'date_published' => $page->getDatePublished(),
            'date_modified' => $page->getDateModified()
        ];
        return $this->json($data);
    }

    #[Route('/api/page',  methods: ['POST'])]
    public function post_page(Request $request): JsonResponse {
        $data = array('type' => 'error', 'content' => '');

        $title = $request->request->get('title');
        $data['content'] = $title;

        return $this->json($data);
    }

    #[Route('/api/page',  methods: ['PUT'])]
    public function put_page(Request $request): JsonResponse {
        $data = array('type' => 'error', 'content' => '');

        $id = $request->query->get('id');
        $title = $request->request->get('title');
        $data['content'] = $id . $title;

        return $this->json($data);
    }

    #[Route('/api/page',  methods: ['DELETE'])]
    public function delete_page(Request $request): JsonResponse {
        $data = array('type' => 'error', 'content' => '');

        $id = $request->query->get('id');
        $data['content'] = $id;
        
        return $this->json($data);
    }
}
