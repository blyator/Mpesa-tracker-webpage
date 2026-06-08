pipeline {
    agent any

    stages {
        stage('Pull') {
            steps {
                git branch: 'main',
                    url: 'https://github.com/blyator/Mpesa-tracker-webpage.git'
            }
        }

        stage('Build Image') {
            steps {
                sh 'docker build -t mpesa-dashboard .'
            }
        }

        stage('Deploy') {
            steps {
                sh 'cd /root/Mpesa-tracker-webpage && docker compose down || true'
                sh 'cd /root/Mpesa-tracker-webpage && docker compose up -d'
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