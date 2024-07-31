const baseUrl = 'http://localhost:7071/api/todos';

const createElement = (tag, id = null, className = '', textContent = '', action = null) => {
    const element = document.createElement(tag);
    if (id) element.id = id;
    element.className = className;
    element.textContent = textContent;
    if (typeof action === 'function') {
        element.addEventListener('click', action);
    }
    return element;
}

async function fetchTodos() {
    try {
        const response = await fetch(baseUrl);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const todos = await response.json();
        const todoList = document.getElementById('todo-list');
        todoList.innerHTML = '';

        todos.forEach(todo => {
            const listItem = createElement('li', todo.id, 'todo');
            const todoSpan = createElement('span', `todo-${todo.id}`, 'todo', todo.task);
            const updateButton = createElement('span', `update-${todo.id}`, 'button', 'Done', updateTodo);
            const deleteButton = createElement('span', `delete-${todo.id}`, 'button', 'Delete', deleteTodo);
            if (todo.isComplete) {
                todoSpan.classList.add('done');
            }
            listItem.append(todoSpan, updateButton, deleteButton);

            todoList.appendChild(listItem);
        });

    } catch (error) {
        console.error('Fetch error:', error);
    }
}

async function addTodo(event) {
    event.preventDefault();
    const taskInput = document.getElementById('todo-task');
    const task = taskInput.value;

    try {
        const response = await fetch(baseUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ task })
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        taskInput.value = '';
        fetchTodos();
    } catch (error) {
        console.error('Add todo error:', error);
    }
}

document.getElementById('todo-form').addEventListener('submit', addTodo);




async function updateTodo(event) {
    const id = event.target.id.split('-')[1];

    try {
        const response = await fetch(`${baseUrl}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ "IsComplete": true })
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        fetchTodos();
    } catch (error) {
        console.error('Update todo error:', error);
    }
}

async function deleteTodo(event) {
    const id = event.target.id.replace('delete-', '');

    try {
        const response = await fetch(`${baseUrl}/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        fetchTodos();
    } catch (error) {
        console.error('Delete todo error:', error);
    }
}

fetchTodos();
