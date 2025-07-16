CREATE TABLE contactos_crm (
    co_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    co_nombre VARCHAR(255) NULL,
    co_mail VARCHAR(255) NULL,
    co_telefono VARCHAR(50) NULL,
    co_ruc VARCHAR(20) NULL,
    co_notas TEXT NULL,
    co_empresa VARCHAR(255) NULL,
    co_cargo VARCHAR(100) NULL,
    co_fecha_contacto DATETIME NOT NULL,
    co_es_cliente INT UNSIGNED NOT NULL DEFAULT 0,
    co_departamento INT UNSIGNED NOT NULL,
    co_ciudad INT UNSIGNED NOT NULL,
    co_zona INT UNSIGNED NOT NULL,
    co_direccion VARCHAR(500) NULL,
    co_estado INT NOT NULL DEFAULT 1,
    co_operador INT UNSIGNED NOT NULL,
    co_general INT NOT NULL DEFAULT 0,
    
    -- Claves foráneas
    FOREIGN KEY (co_departamento) REFERENCES departamentos(dep_codigo),
    FOREIGN KEY (co_ciudad) REFERENCES ciudades(ciu_codigo),
    FOREIGN KEY (co_zona) REFERENCES zonas(zo_codigo),
    FOREIGN KEY (co_operador) REFERENCES operadores(op_codigo)
);

-- Índices para mejorar el rendimiento
CREATE INDEX idx_contactos_crm_departamento ON contactos_crm(co_departamento);
CREATE INDEX idx_contactos_crm_ciudad ON contactos_crm(co_ciudad);
CREATE INDEX idx_contactos_crm_zona ON contactos_crm(co_zona);
CREATE INDEX idx_contactos_crm_estado ON contactos_crm(co_estado);
CREATE INDEX idx_contactos_crm_fecha ON contactos_crm(co_fecha_contacto);


CREATE TABLE estados_crm (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(255) NULL
);

-- Índice para mejorar el rendimiento en búsquedas por descripción
CREATE INDEX idx_estados_crm_descripcion ON estados_crm(descripcion);

CREATE TABLE oportunidades_crm (
    op_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    op_cliente INT UNSIGNED NOT NULL,
    op_titulo VARCHAR(255) NULL,
    op_descripcion TEXT NULL,
    op_valor_negociacion DECIMAL(15,2) NOT NULL DEFAULT 0.00,
    op_fecha_inicio DATETIME NOT NULL,
    op_fecha_fin DATETIME NULL,
    op_operador INT NOT NULL,
    op_estado INT UNSIGNED NOT NULL,
    op_general INT NOT NULL DEFAULT 0,
    
    -- Claves foráneas
    FOREIGN KEY (op_cliente) REFERENCES contactos_crm(co_codigo),
    FOREIGN KEY (op_operador) REFERENCES operadores(op_codigo),
    FOREIGN KEY (op_estado) REFERENCES estados_crm(id)
);

-- Índices para mejorar el rendimiento
CREATE INDEX idx_oportunidades_crm_cliente ON oportunidades_crm(op_cliente);
CREATE INDEX idx_oportunidades_crm_operador ON oportunidades_crm(op_operador);
CREATE INDEX idx_oportunidades_crm_estado ON oportunidades_crm(op_estado);
CREATE INDEX idx_oportunidades_crm_fecha_inicio ON oportunidades_crm(op_fecha_inicio);
CREATE INDEX idx_oportunidades_crm_valor ON oportunidades_crm(op_valor_negociacion);

CREATE TABLE tareas_crm (
    ta_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    ta_titulo VARCHAR(255) NULL,
    ta_descripcion TEXT NULL,
    ta_resultado TEXT NULL,
    ta_fecha DATETIME NOT NULL,
    ta_oportunidad INT UNSIGNED NOT NULL,
    ta_tipo_tarea INT UNSIGNED NOT NULL,
    ta_fecha_limite DATETIME NULL,
    ta_estado INT NOT NULL DEFAULT 1,
    
    -- Clave foránea
    FOREIGN KEY (ta_oportunidad) REFERENCES oportunidades_crm(op_codigo)
);

-- Índices para mejorar el rendimiento
CREATE INDEX idx_tareas_crm_oportunidad ON tareas_crm(ta_oportunidad);
CREATE INDEX idx_tareas_crm_fecha ON tareas_crm(ta_fecha);
CREATE INDEX idx_tareas_crm_estado ON tareas_crm(ta_estado);
CREATE INDEX idx_tareas_crm_tipo_tarea ON tareas_crm(ta_tipo_tarea);


CREATE TABLE tipo_tarea_crm (
    tipo_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    tipo_descripcion VARCHAR(255) NULL,
    tipo_estado INT UNSIGNED NOT NULL DEFAULT 1
);

-- Índice para mejorar el rendimiento en búsquedas por descripción
CREATE INDEX idx_tipo_tarea_crm_descripcion ON tipo_tarea_crm(tipo_descripcion);
CREATE INDEX idx_tipo_tarea_crm_estado ON tipo_tarea_crm(tipo_estado);


-- Agregar la clave foránea faltante en tareas_crm
ALTER TABLE tareas_crm 
ADD CONSTRAINT fk_tareas_tipo_tarea 
FOREIGN KEY (ta_tipo_tarea) REFERENCES tipo_tarea_crm(tipo_codigo);



-- Agregar la clave foránea para relacionar tareas_crm con tipo_tarea_crm
ALTER TABLE tareas_crm 
ADD CONSTRAINT fk_tareas_tipo_tarea 
FOREIGN KEY (ta_tipo_tarea) REFERENCES tipo_tarea_crm(tipo_codigo);

-- Agregar un índice adicional para optimizar las consultas por tipo de tarea
CREATE INDEX idx_tareas_crm_tipo_tarea_fk ON tareas_crm(ta_tipo_tarea);