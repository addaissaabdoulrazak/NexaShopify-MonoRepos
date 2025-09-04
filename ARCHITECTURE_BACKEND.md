# Documentation de l'Architecture Backend NexaShopify

## Vue d'ensemble
L'architecture backend de NexaShopify est organisée en trois principaux dossiers : **Core**, **Infrastructure**, et **Web**. Cette structure suit une approche modulaire et en couches, facilitant la séparation des responsabilités, la maintenabilité et l'évolutivité de l'application.

---

## 1. Dossier `Core`
Le dossier `Core` contient la logique métier fondamentale, les modèles de domaine, les exceptions, les interfaces et les contrats partagés. Il est subdivisé en plusieurs sous-projets :

- **NexaShopify.Core** :
  - Contient la logique métier principale, les exceptions personnalisées (ex : `NotFoundException`, `UnauthorizedException`), et les modèles de réponse.
  - Dossier `Apps/` : organisation potentielle par modules métiers.
- **NexaShopify.Core.Common** :
  - Définit des modèles et objets partagés entre plusieurs modules.
- **NexaShopify.Core.Identity** :
  - Gère les modèles, énumérations et gestion des utilisateurs et rôles.
  - Dossiers `Handlers/`, `Models/`, `Enums/` pour la gestion de l'identité.
- **NexaShopify.Core.SharedKernel** :
  - Contient les interfaces, exceptions et éléments transverses réutilisables dans tout le backend.
- **NexaShopify.Core.Shop** :
  - Gère la logique métier liée aux boutiques, produits, catégories, etc.
  - Dossiers `Handlers/`, `Helpers/`, `Models/`, `Enums/` pour la gestion du shop.

**Rôle principal :**
- Définit le cœur métier, les entités, les règles de gestion et les contrats d’interface.
- Ne dépend d’aucune autre couche de l’application.

---

## 2. Dossier `Infrastructure`
Le dossier `Infrastructure` implémente les détails techniques et l’accès aux données. Il est divisé en deux sous-projets principaux :

- **Infrastructure.Data** :
  - Fournit l’accès aux données (DAL) via des classes d’accès (`Access/`) et des entités de base de données (`Entities/`).
  - Dossiers spécialisés pour chaque domaine (ex : `Store/`, `COR/`).
  - Dossier `Interfaces/` pour les contrats d’accès aux données.
- **Infrastructure.Service** :
  - Contient les services techniques transverses (ex : logging, gestion des transactions).
  - Dossier `Logging/` pour la gestion des logs, `Utils/` pour les utilitaires techniques.

**Rôle principal :**
- Implémente les interfaces définies dans le Core.
- Gère la persistance, l’accès aux bases de données, les services externes et les aspects techniques.
- Peut dépendre du Core, mais jamais l’inverse.

---

## 3. Dossier `Web`
Le dossier `Web` contient l’API principale exposée aux clients (frontends, applications tierces, etc.).

- **NexaShopify.API** :
  - Projet ASP.NET Core Web API.
  - Dossiers `Controllers/` pour les points d’entrée API, organisés par domaine (ex : `StoreManagement`, `Settings`, `ManagementOverview`).
  - Dossiers `Models/` pour les modèles de requête/réponse spécifiques à l’API.
  - Dossiers `Extensions/`, `Areas/` pour l’organisation avancée des fonctionnalités.
  - Fichiers de configuration (`appsettings.json`, `nlog.config`, etc.).

**Rôle principal :**
- Expose les endpoints RESTful.
- Orchestration des appels entre la couche Core et Infrastructure.
- Gère la sécurité, la validation, la sérialisation et la configuration de l’application.

---

## 4. Relations entre les couches
- **Web** dépend de **Core** et **Infrastructure**.
- **Infrastructure** dépend de **Core**.
- **Core** est totalement indépendant.

Ce découpage permet de :
- Faciliter les tests unitaires (Core testable indépendamment).
- Remplacer facilement l’implémentation technique (Infrastructure) sans impacter le métier.
- Maintenir une API claire et évolutive.

---

## 5. Schéma simplifié

```
[ Web (API) ]
      |
      v
[ Core (Métier, Contrats) ]
      ^
      |
[ Infrastructure (Données, Services) ]
```

---

## 6. Bonnes pratiques
- **Séparation stricte des responsabilités** : chaque couche a un rôle précis.
- **Utilisation des interfaces** pour l’injection de dépendances et la testabilité.
- **Organisation modulaire** pour faciliter l’évolution et la maintenance.

---

## 7. Conclusion
Cette architecture modulaire et en couches permet à NexaShopify d’être robuste, évolutif et facilement maintenable, tout en respectant les standards modernes du développement .NET.
