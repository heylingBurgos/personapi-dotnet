-- Crear objetos de prueba para la tabla Personas
INSERT INTO persona (Nombre, Apellido, Genero, edad)
VALUES ('Joling', 'Lopez Burgos', 'F', 20),
       ('Heyling', 'Burgos', 'F', 22),
       ('Jose', 'Lopez', 'M', 23);

-- Crear objetos de prueba para la tabla Telefonos
INSERT INTO telefono (num, oper, duenio)
VALUES ('555-1234', 'Movistar', 1),
       ('555-5678', 'Tigo', 1),
       ('555-9876', 'Claro', 2),
       ('555-4321', 'Claro', 3),
       ('555-1111', 'Wom', 3),
       ('555-2222', 'Tigo', 3);

-- Crear objetos de prueba para la tabla Profesiones
INSERT INTO profesion(id, nom, des)
VALUES (1, 'Programador', 'Programas cosas geniales'),
       (2, 'Ingeniero', 'Eres un feto'),
       (3, 'Contador','Haces fraude fiscal');

-- Crear objetos de prueba para la tabla Estudios
INSERT INTO Estudios (univer, fecha, cc_per, id_prof)
VALUES ('Javeriana', '2012-06-30', 1, 1),
       ('Harvard', '2010-06-30', 2, 2),
       ('Sena', '2015-12-15', 3, 3);
       