pipeline {
    agent any

    stages {
        stage('Pull') {
            steps {
                git branch: 'main',
                    url: 'https://github.com/blyator/Mpesa-tracker-webpage.git'
            }
        }

        stage('Build CSS') {
            steps {
                sh 'npm install'
                sh 'npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/site.css'
            }
        }

        stage('Build Image') {
            steps {
                sh 'docker build -t mpesa-dashboard .'
            }
        }

        stage('Deploy') {
            steps {
                sh 'docker compose down || true'
                sh 'docker compose up -d'
            }
        }
    }

    post {
        success {
            echo 'Deployed successfully'
        }
        failure {
            echo 'Build failed'
        }
    }
}