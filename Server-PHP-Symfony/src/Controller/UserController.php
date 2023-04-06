<?php
namespace App\Controller;

use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Doctrine\ORM\EntityManagerInterface;
use App\Entity\User;
use App\Service\JwtService;

class UserController extends AbstractController {

    #[Route('/api/user')]
    public function user(): Response {
        return new Response(
            '<html><body> Test Page </body></html>'
        );
    }

    #[Route('/api/user/login', methods: ['GET'])]
    public function user_login(Request $request, EntityManagerInterface $entityManager, JwtService $jwtService): JsonResponse {
        $userRepository = $entityManager->getRepository(User::class);
        $data = array('type' => 'error', 'content' => 'Invalid Credentials provided.');
        $username = $request->query->get('username');
        $password = $request->query->get('password');
        $user = $userRepository->findOneBy([
            'username' => $username,
            'password' => $password,
        ]);
        if(!$user) {
            return $this->json($data);
        }
        $key = $this->getParameter('env.secret');
        $token = $jwtService->generate_token($user->getId(), $key);
        return $this->json(array('type' => 'success', 'content' => $token));
    }

    #[Route('/api/user/logout')]
    public function user_logout(Request $request, JwtService $jwtService): JsonResponse {
        $token = $request->query->get('token');
        $key = $this->getParameter('env.secret');
        if (!$token || !$jwtService->verify_token($token, $key)) {
            return $this->json(array('type' => 'error', 'content' => 'Please login.'));
        }
        return $this->json(array('type' => 'success', 'content' => $jwtService->refresh_token($token, $key, time() - 3600)));
    }
}
