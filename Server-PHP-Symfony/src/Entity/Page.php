<?php
// src/Entity/Page.php
namespace App\Entity;

use App\Repository\PageRepository;
use Doctrine\ORM\Mapping as ORM;
use Doctrine\DBAL\Types\Types;

#[ORM\Entity(repositoryClass: PageRepository::class)]
class Page {
    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column(type: Types::INTEGER)]
    private ?int $id = null;

    #[ORM\Column(type: Types::INTEGER)]
    private ?int $user_id = null;

    #[ORM\Column]
    private ?string $url = null;

    #[ORM\Column]
    private ?string $title = null;

    #[ORM\Column(type: Types::TEXT)]
    private ?string $content = null;

    #[ORM\Column(type: Types::INTEGER)]
    private ?int $date_published = 0;

    #[ORM\Column(type: Types::INTEGER)]
    private ?int $date_modified = 0;

    public function getId(): ?int
    {
        return $this->id;
    }

    public function getUserId(): ?int
    {
        return $this->user_id;
    }

    public function setUserId($user_id): self
    {
        $this->user_id = $user_id;
        return $this;
    }

    public function getUrl(): ?string
    {
        return $this->url;
    }

    public function setUrl($url): self
    {
        $this->url = $url;
        return $this;
    }

    public function getTitle(): ?string
    {
        return $this->title;
    }

    public function setTitle($title): self
    {
        $this->title = $title;
        return $this;
    }

    public function getContent(): ?string
    {
        return $this->content;
    }

    public function setContent($content): self
    {
        $this->content = $content;
        return $this;
    }

    public function getDatePublished(): ?int
    {
        return $this->date_published;
    }

    public function setDatePublished($date_published): self
    {
        $this->date_published = $date_published;
        return $this;
    }

    public function getDateModified(): ?int
    {
        return $this->date_modified;
    }

    public function setDateModified($date_modified): self
    {
        $this->date_modified = $date_modified;
        return $this;
    }
}