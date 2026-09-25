-- 1. Criação da tabela de status do item
CREATE TABLE IF NOT EXISTS tb_item_status (
    id INT PRIMARY KEY,
    status VARCHAR(50) NOT NULL UNIQUE
);

-- 2. Inserção dos registros baseados no enum ItemStatus
INSERT INTO tb_item_status (id, status) VALUES
    (10, 'ENCONTRADO'),
    (20, 'PERDIDO'),
    (30, 'DEVOLVIDO')
ON CONFLICT (id) DO NOTHING;

-- 3. Criação da tabela tb_item com a referência id_status
CREATE TABLE IF NOT EXISTS tb_item (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    description TEXT,
    category VARCHAR(100),
    found_location VARCHAR(200),
    found_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_status INT NOT NULL DEFAULT 10,
    person_who_found VARCHAR(150),
    contact_who_found VARCHAR(150),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_tb_item_status
        FOREIGN KEY (id_status) 
        REFERENCES tb_item_status (id)
);

-- 4. Dados iniciais: livros de programação encontrados na biblioteca
INSERT INTO public.tb_item (
    name,
    description,
    category,
    found_location,
    found_date,
    id_status,
    person_who_found,
    created_at,
    updated_at
)
SELECT
    seed.name,
    'Livro de programação encontrado na biblioteca',
    'Livros',
    'Biblioteca',
    CURRENT_TIMESTAMP,
    10,
    seed.person_who_found,
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP
FROM (VALUES
    ('Implementando Domain-Driven Design', 'Evaldo Rodrigues'),
    ('Código Limpo', 'Alan'),
    ('Padrões de Projeto', 'Helder Lima'),
    ('Fundamentos de Arquitetura de Software', 'Levi Alves'),
    ('Engenharia de Software Moderna', 'Murilo Aragão')
) AS seed(name, person_who_found)
WHERE NOT EXISTS (
    SELECT 1
    FROM public.tb_item AS existing
    WHERE existing.name = seed.name
      AND existing.person_who_found = seed.person_who_found
);