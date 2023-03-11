<?php
namespace App\Controller;

use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Annotation\Route;

class UserController {

    #[Route('/api/user')]
    public function user(): Response {
        return new Response(
            '<html><body> Test </body></html>'
        );
    }

    #[Route('/api/user/login')]
    public function user_login(): Response {
        return new Response(
            '<html><body> Test </body></html>'
        );
    }

    #[Route('/api/user/logout')]
    public function user_logout(): Response {
        return new Response(
            '<html><body> Test </body></html>'
        );
    }
}
