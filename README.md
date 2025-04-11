
# Ambev Developer Evaluation

This project is part of a technical evaluation for senior developer candidates. It assesses core skills such as clean architecture, testing, API design, documentation, and maintainability.

---

## 📌 Project Goals

- Demonstrate backend skills using a real-world-like application.
- Follow best practices in code structure and organization.
- Provide clean, testable, and documented code.

---

## 📂 Project Structure

```
root
├── src/                # Source code
├── tests/              # Unit and integration tests
├── .github/            # GitHub Actions and configurations
├── docker-compose.yml  # Docker configuration
├── README.md           # This file
```

---

## 🛠️ Tech Stack

The main technologies used in this project include:

- **.NET** (C#)
- **Entity Framework Core**
- **Docker**
- **XUnit** for testing
- **Swagger** for API documentation

---

## 🚀 How to Run the Project

You can run the application locally using Docker:

```bash
docker-compose up --build
```

The backend services will be available at `http://localhost:5119`.

To access Swagger API docs:

```
http://localhost:5119/swagger
```

---

## 🧪 Running Tests

To run the tests:

```bash
.\coverage-report.bat
```

Test results will be available in the `TestResults/` directory.

---

## 📚 API Documentation

The system includes several APIs, each with its own endpoints:

- **Authentication** (`/auth`)
- **Users** (`/users`)
- **Products** (`/products`)
- **Carts** (`/carts`)

Each API supports pagination using `_page` and `_size` query parameters.

You can find full documentation inside the `doc/` folder, or access Swagger when running the app.

---

## 📦 Frameworks

We use a curated set of frameworks that help us ensure scalability, maintainability, and performance. See `frameworks.md` for details.

---

## 📁 Additional Docs

- [`overview.md`](./.doc/overview.md): Project overview
- [`tech-stack.md`](./.doc/tech-stack.md): Tech stack details
- [`project-structure.md`](./.doc/project-structure.md): Project layout and conventions

---

## 📌 Evaluation Criteria

- Code readability and maintainability
- Proper use of design patterns (e.g., DDD)
- Test coverage
- API quality and documentation

---

## 🧑‍💻 Author
Celso Catarino
https://www.linkedin.com/in/celso-catarino-50b241186/
This code was submitted as part of a technical challenge for Ambev. Please refer to the documentation folder for full specs and expected features.

---
