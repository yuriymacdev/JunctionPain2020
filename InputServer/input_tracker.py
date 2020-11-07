#!/usr/bin/env python3

import os
import sys
import socket
import time
import cv2
import logging

from base_pointer import BasePointer
from gaze_estimation import GazeEstimation
from sys import platform


class Config:
    def __init__(self):
        self.model1 = "face-detection-adas-0001.xml"
        self.model2 = "head-pose-estimation-adas-0001.xml"
        self.model3 = "landmarks-regression-retail-0009.xml"
        self.model4 = "gaze-estimation-adas-0002.xml"
        self.device = "CPU"
        # self.device = "GPU"
        self.prob_threshold = 0.5

        self.source = 0

        self.address = "127.0.0.1"
        self.port = 11000

        self.output_intermediate_model = True

class InputReporter:
    def __init__(self, config):
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM, 0)
        self.address = config.address
        self.port = config.port

    def sendReport(self, ypr):
        report = "{} {} {}".format(ypr[2], ypr[1], ypr[0])
        self.socket.sendto(report.encode('utf-8'), (self.address, self.port))
        # data, address = s.recvfrom(4096)

def crop_face(coords, frame, output_intermediate_model):
    delta_y = coords[0][3] - coords[0][1]
    delta_x = coords[0][2] - coords[0][0]
    frame = frame[coords[0][1]:coords[0][1]+delta_y, coords[0][0]:coords[0][0]+delta_x]
    # if output_intermediate_model == 'true':
        # cv2.imwrite('output_image_crop_face.jpg', frame)
    return frame

def infer_on_stream(args):
    output_intermediate_model = args.output_intermediate_model

    cap = cv2.VideoCapture(0)
    cap.set(cv2.CAP_PROP_FPS, 60)
    width = int(cap.get(3))
    height = int(cap.get(4))
    fps = int(cap.get(cv2.CAP_PROP_FPS))
    print("Capture FPS: {}".format(fps))

    data_link = InputReporter(args)

    try:
        infer_network_face_detection = BasePointer()
        infer_network_head_pose_estimation = BasePointer()
        infer_network_landmarks_regression_retail = BasePointer()
        infer_network_gaze_estimation = GazeEstimation()
    except:
        logging.error("Error in initializing models")
        exit(1)
    ### TODO: Load the model through `infer_network_face_detection` ###
    try:
        start_loading_time_face_detection = time.time()
        infer_network_face_detection.load_model(args.model1, args.device)
        load_model_face_detection_time_taken = time.time() - start_loading_time_face_detection

        start_loading_time_head_pose_estimation = time.time()
        infer_network_head_pose_estimation.load_model(args.model2, args.device)
        load_model_head_pose_estimation_time_taken = time.time() - start_loading_time_head_pose_estimation

        start_loading_time_landmarks_regression_retail = time.time()
        infer_network_landmarks_regression_retail.load_model(args.model3, args.device)
        load_model_landmarks_regression_retail_time_taken = time.time() - start_loading_time_landmarks_regression_retail

        start_loading_time_gaze_estimation = time.time()
        infer_network_gaze_estimation.load_model(args.model4, args.device)
        load_model_gaze_estimation_time_taken = time.time() - start_loading_time_gaze_estimation
    except:
        logging.error("Error in loading the models")
        exit(1)

    logging.debug("Loading times for facial detection : {} , landmark detection : {} , head pose detection : {} , gaze estimation : {} ".format(load_model_face_detection_time_taken, load_model_landmarks_regression_retail_time_taken, load_model_head_pose_estimation_time_taken, load_model_gaze_estimation_time_taken))

    #     out = cv2.VideoWriter('out.mp4', CODEC, fps, (width,height))

    total_time_taken_to_infer_inf_face_detection = 0
    total_time_taken_to_infer_landmarks_regression_retail = 0
    total_time_taken_to_infer_inf_head_pose_estimation = 0
    total_time_taken_to_infer_gaze_estimation = 0


    while True:
        try:
            flag, frame  = cap.read()
            if not flag:
                break

            frame= cv2.flip(frame, 1)
            start_inf_face_detection = time.time()
            outputs_face_detection = infer_network_face_detection.predict(frame)
            time_taken_to_infer_inf_face_detection = time.time() - start_inf_face_detection
            coords, frame = infer_network_face_detection.preprocess_output_face_detection(outputs_face_detection, width, height, args.prob_threshold, frame)
            if output_intermediate_model == 'true':
                out.write(frame)

            frame_crop_face = crop_face(coords, frame, output_intermediate_model)

            start_inf_head_pose_estimation = time.time()
            outputs_head_pose_estimation = infer_network_head_pose_estimation.predict(frame_crop_face)
            time_taken_to_infer_inf_head_pose_estimation = time.time() - start_inf_head_pose_estimation

            yaw, pitсh, roll = infer_network_head_pose_estimation.preprocess_output_head_pose_estimation(outputs_head_pose_estimation, frame_crop_face)
            head_pose_angles = [yaw, pitсh, roll]
            print(head_pose_angles)
            data_link.sendReport(head_pose_angles)

            height_crop_face = coords[0][3] - coords[0][1]
            width_crop_face = coords[0][2] - coords[0][0]

            start_inf_landmarks_regression_retail = time.time()
            outputs_landmarks_regression_retail = infer_network_landmarks_regression_retail.predict(frame_crop_face)
            time_taken_to_infer_landmarks_regression_retail = time.time() - start_inf_landmarks_regression_retail

            coord_landmarks_regression_retail = infer_network_landmarks_regression_retail.preprocess_output_landmarks_regression_retail(outputs_landmarks_regression_retail, width_crop_face, height_crop_face, args.prob_threshold, frame)
            center_left_eye = ((coords[0][0]+coord_landmarks_regression_retail[0]).astype(int)[0, 0],(coords[0][1]+coord_landmarks_regression_retail[1]).astype(int)[0, 0])
            center_right_eye = ((coords[0][0]+coord_landmarks_regression_retail[2]).astype(int)[0, 0],(coords[0][1]+coord_landmarks_regression_retail[3]).astype(int)[0, 0])


            xmin_left_eye = center_left_eye[0] - 30
            ymin_left_eye = center_left_eye[1] - 30
            xmax_left_eye = center_left_eye[0] + 30
            ymax_left_eye = center_left_eye[1] + 30
            xmin_right_eye = center_right_eye[0] - 30
            ymin_right_eye = center_right_eye[1] - 30
            xmax_right_eye = center_right_eye[0] + 30
            ymax_right_eye = center_right_eye[1] + 30

            # print(center_left_eye)
            frame_landmarks_regression_retail  = cv2.circle(frame,center_left_eye, 2, (0, 255, 0), thickness=3)
            frame_landmarks_regression_retail  = cv2.circle(frame,center_right_eye , 2, (0, 255, 0), thickness=3)
            box_left_eye = cv2.rectangle(frame, (xmin_left_eye, ymin_left_eye), (xmax_left_eye, ymax_left_eye), (0,255,0), 3)
            box_right_eye = cv2.rectangle(frame, (xmin_right_eye, ymin_right_eye), (xmax_right_eye, ymax_right_eye), (0,255,0), 3)
            if output_intermediate_model == 'true':
                out.write(frame_landmarks_regression_retail)

            ### TODO: Start inference for gaze estimation ###
            start_inf_gaze_estimation = time.time()
            outputs_gaze_estimation = infer_network_gaze_estimation.predict(box_left_eye, box_right_eye, head_pose_angles)
            time_taken_to_infer_gaze_estimation = time.time() - start_inf_gaze_estimation

            total_time_taken_to_infer_inf_face_detection = time_taken_to_infer_inf_face_detection + total_time_taken_to_infer_inf_face_detection
            total_time_taken_to_infer_landmarks_regression_retail = time_taken_to_infer_landmarks_regression_retail + total_time_taken_to_infer_landmarks_regression_retail
            total_time_taken_to_infer_inf_head_pose_estimation = time_taken_to_infer_inf_head_pose_estimation + total_time_taken_to_infer_inf_head_pose_estimation
            total_time_taken_to_infer_gaze_estimation = time_taken_to_infer_gaze_estimation + total_time_taken_to_infer_gaze_estimation

            # if output_intermediate_model == 'true':
            if True:
                cv2.putText(frame,("Yaw: " + str(int(yaw))), (10,10+20), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)
                cv2.putText(frame,("Pitch: " + str(int(pitсh))), (10,50+20), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)
                cv2.putText(frame,("Roll: " + str(int(roll))), (10, 90+20), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)
                cv2.putText(frame,("detection: {:.2f}ms".format(time_taken_to_infer_gaze_estimation*1000)), (10, 130+20), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 255), 2)

            arrow = 100
            g_x = int(outputs_gaze_estimation[0]*arrow)
            g_y = int(-(outputs_gaze_estimation[1])*arrow)

            print(g_x, g_y)

            frame = cv2.arrowedLine(frame, (center_left_eye), ((center_left_eye[0]+g_x), (center_left_eye[1]+g_y)), (0, 0, 255), 3)
            frame = cv2.arrowedLine(frame, (center_right_eye), ((center_right_eye[0]+g_x), (center_right_eye[1]+g_y)), (0, 0, 255), 3)

            # if output_intermediate_model == 'true':
            cv2.imshow("gaze", frame)
                # cv2.waitKey
                    # out.write(frame)

            # move(outputs_gaze_estimation[0], outputs_gaze_estimation[1])

            key_pressed = cv2.waitKey(1)
            if key_pressed == 27:
                break
        except IndexError as e:
            continue
        except cv2.error as e:
            continue
        except Exception as e:
            raise e
    feed.close()

    logging.debug("total inference times for facial detection : {} , landmark detection : {} , head pose detection : {} , gaze estimation : {} ".format(total_time_taken_to_infer_inf_face_detection, total_time_taken_to_infer_landmarks_regression_retail, total_time_taken_to_infer_inf_head_pose_estimation, total_time_taken_to_infer_gaze_estimation))
    # if output_intermediate_model == 'true':
    #     out.release()
    #cap.release()
    cv2.destroyAllWindows()

def main():
    logging.basicConfig(filename="app.log", level=logging.DEBUG, format='%(asctime)s:%(levelname)s:%(message)s')

    args = Config()
    infer_on_stream(args)

if __name__ == '__main__':
    main()
