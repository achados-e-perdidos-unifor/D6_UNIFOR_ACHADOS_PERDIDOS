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