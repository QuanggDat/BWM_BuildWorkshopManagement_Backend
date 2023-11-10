pipeline {
    agent any
    stages {
        stage("Verify tooling") {
            steps {
                sh '''
                    docker version
<<<<<<< HEAD
=======
                    docker info
>>>>>>> f7b28846f66237bade012e7ef8dbebae8d84c3ec
                    docker-compose version
                '''
            }
        }
<<<<<<< HEAD
        stage('Start container') {
=======
        stage('Check container') {
>>>>>>> f7b28846f66237bade012e7ef8dbebae8d84c3ec
            steps {
                script {
                    try {
                        // Your build commands or steps that might fail
                        sh 'docker stop workshop-management-system-jama'
                        sh 'docker rm workshop-management-system-jama'
                    } catch (Exception e) {
                        echo "Cannot find container workshop-management-system-jama"
                    }
                }
<<<<<<< HEAD
                sh 'docker-compose up --build --wait'
=======
            }
        }
        stage('Start container') {
            steps {
                sh "docker-compose up --build -d"
>>>>>>> f7b28846f66237bade012e7ef8dbebae8d84c3ec
                sh 'docker-compose ps'
            }
        }
    }
}
