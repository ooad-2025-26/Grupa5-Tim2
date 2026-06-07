DELETE FROM Knjiga;

INSERT INTO Knjiga (Naziv, Autor, ISBN, Kategorija, GodinaIzdanja, BrojStranica, Izdavac, Naslovnica, Status)
VALUES 
('Mali Princ', 'Antoine de Saint-Exupéry', '978-9958478100', 'Fikcija', 1943, 96, 'Mladost', 'https://upload.wikimedia.org/wikipedia/en/0/05/Littleprince.jpg', 0),
('Na Drini Ćuprija', 'Ivo Andrić', '978-9958034022', 'Roman', 1945, 320, 'Svjetlost', 'https://upload.wikimedia.org/wikipedia/commons/0/0f/Na_Drini_%C4%87uprija.jpg', 0),
('1984', 'George Orwell', '978-0451524935', 'Distopija', 1949, 328, 'Secker', 'https://upload.wikimedia.org/wikipedia/en/c/c3/1984first.jpg', 0),
('Tvrđava', 'Meša Selimović', '978-9958034039', 'Roman', 1970, 424, 'Svjetlost', 'https://upload.wikimedia.org/wikipedia/sr/2/2e/Tvrdjava.jpg', 0),
('Harry Potter', 'J.K. Rowling', '978-0439708180', 'Fantazija', 1997, 309, 'Bloomsbury', 'https://upload.wikimedia.org/wikipedia/en/6/6b/Harry_Potter_and_the_Philosopher%27s_Stone_Book_Cover.jpg', 0);