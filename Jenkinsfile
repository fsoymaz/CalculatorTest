pipeline {
    agent any
    
    environment {
        CREDENTIALS_ID = '31'
        GITHUB_REPO_BACKEND = 'git@github.com:fsoymaz/Calculator.git'
        NUGET_FEED = "${WORKSPACE}/nuget_packages"
    }
    
    stages {
        stage('Checkout CalculatorTest') {
            steps {
                echo '📥 CalculatorTest Repository klone ediliyor...'
                checkout scm
            }
        }
        
        stage('Clone Calculator Backend') {
            steps {
                echo '📦 Calculator Backend repository klone ediliyor...'
                sh '''
                    cd ${WORKSPACE}
                    rm -rf Calculator_Lib || true
                    git clone ${GITHUB_REPO_BACKEND} Calculator_Lib
                '''
            }
        }
        
        stage('Build NuGet Package') {
            steps {
                echo '🔨 CalculatorLib NuGet package oluşturuluyor...'
                sh '''
                    mkdir -p ${NUGET_FEED}
                    cd ${WORKSPACE}/Calculator_Lib/CalculatorLib
                    dotnet pack -c Release -o ${NUGET_FEED}
                '''
            }
        }
        
        stage('Setup Dependencies') {
            steps {
                echo '📋 NuGet feed ekleniyor...'
                sh '''
                    cd ${WORKSPACE}/CalculatorTests
                    dotnet nuget add source ${NUGET_FEED} --name jenkins-nuget || true
                    dotnet restore
                '''
            }
        }
        
        stage('Run Unit Tests') {
            steps {
                echo '🧪 Unit testler çalıştırılıyor...'
                sh '''
                    cd ${WORKSPACE}/CalculatorTests
                    dotnet test --no-restore --verbosity normal
                '''
            }
        }
        
        stage('Test Decision') {
            steps {
                echo '✅ Test sonuçları kontrol ediliyor...'
                script {
                    try {
                        sh '''
                            cd ${WORKSPACE}/CalculatorTests
                            dotnet test --no-restore
                        '''
                        echo '✅ TÜM TESTLER BAŞARILI!'
                        env.TEST_PASSED = 'true'
                    } catch (Exception e) {
                        echo '❌ TESTLER BAŞARILI DEĞİL! Push yapılmayacak!'
                        env.TEST_PASSED = 'false'
                        error 'Tests failed - Push blocked'
                    }
                }
            }
        }
        
        stage('Push to Calculator Repo') {
            when {
                expression { env.TEST_PASSED == 'true' }
            }
            steps {
                echo '🚀 Calculator repository\'na push yapılıyor...'
                withCredentials([sshUserPrivateKey(credentialsId: "${CREDENTIALS_ID}", keyFileVariable: 'SSH_KEY')]) {
                    sh '''
                        export GIT_SSH_COMMAND="ssh -i ${SSH_KEY} -o StrictHostKeyChecking=no"
                        cd ${WORKSPACE}/Calculator_Lib
                        git config user.name "Jenkins CI"
                        git config user.email "jenkins@ci.local"
                        git tag -a "tested-${BUILD_NUMBER}" -m "Tests Passed"
                        git push origin main --tags
                    '''
                }
            }
        }
    }
    
    post {
        success {
            echo '✅ PIPELINE BAŞARILI - Testler geçti, Calculator\'a push yapıldı'
        }
        failure {
            echo '❌ PIPELINE BAŞARILI DEĞİL - Testler başarısız, push ENGELLENDI'
        }
    }
}
